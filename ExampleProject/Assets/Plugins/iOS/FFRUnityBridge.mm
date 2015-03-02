//
//  FuffrUnityBridge.m
//  Unity-iPhone
//
//  Created by Andrey Zhukov on 26/08/14.
//
//

#import "FFRUnityBridge.h"
#import "FFRTouchManager.h"

#if defined(DEBUG) || TARGET_IPHONE_SIMULATOR
#   define FFRLog(...) NSLog(__VA_ARGS__)
#else
#   define FFRLog(...)
#endif

extern "C" {
    typedef struct FFRTouchC {
        uint32_t id;
        FFRSide side;
        FFRTouchPhase phase;
        float x;
        float y;
        float normx;
        float normy;
        float prevx;
        float prevy;
    } FFRTouchCStruct;
    
    typedef void (*ReceiveDeviceMessageCallback)(FFRTouchCStruct touches[], int32_t length);
}

@interface FFRUnityBridge()

@property (nonatomic, assign) ReceiveDeviceMessageCallback receiveDeviceMessageCallback;

- (void)startListeningToTouchEventsOnSides:(FFRSide)enabledSides touchesPerSide:(NSUInteger)touchesPerSide;

@end

extern "C" {
    void _StartListeningToTouchEventsOnSides(uint32_t sides, uint32_t maxTouches) {
        [[FFRUnityBridge sharedInstance] startListeningToTouchEventsOnSides:(FFRSide)sides touchesPerSide:(NSUInteger)maxTouches];
    }
    
    void _RegisterRecieveDeviceMessageCallback(ReceiveDeviceMessageCallback callback) {
        [FFRUnityBridge sharedInstance].receiveDeviceMessageCallback = callback;
    }
}

@implementation FFRUnityBridge

+ (instancetype)sharedInstance {
    static dispatch_once_t once;
    static FFRUnityBridge* sharedInstance;
    dispatch_once(&once, ^ { sharedInstance = [[self alloc] init]; });
    return sharedInstance;
}

- (void)dealloc {
    _receiveDeviceMessageCallback = nil;
    [super dealloc];
}

- (void)startListeningToTouchEventsOnSides:(FFRSide)enabledSides touchesPerSide:(NSUInteger)touchesPerSide {
    FFRLog(@"Scanning for Fuffr...");
    
    // Get a reference to the touch manager.
    FFRTouchManager* manager = [FFRTouchManager sharedManager];
    
    [manager
     onFuffrConnected:
     ^{
         FFRLog(@"Fuffr Connected");
         [manager useSensorService:^{
             // Sensor is available, set active sides.
             [[FFRTouchManager sharedManager]
              enableSides: enabledSides
              touchesPerSide: @(touchesPerSide)];
         }];
     }
     onFuffrDisconnected:^{
         FFRLog(@"Fuffr Disconnected");
     }];
    
    // Register methods for touch events. Here the side constants are
    // bit-or:ed to capture touches on all four sides.
    [manager addTouchObserver:self
                   touchBegan:@selector(touchesBegan:)
                   touchMoved:@selector(touchesMoved:)
                   touchEnded:@selector(touchesEnded:)
                        sides:enabledSides];
}

- (void)touchesBegan:(NSSet*)touches {
    [self forwardTouchesToUnity:touches phase:FFRTouchPhaseBegan];
}

- (void)touchesMoved:(NSSet*)touches {
    [self forwardTouchesToUnity:touches phase:FFRTouchPhaseMoved];
}

- (void)touchesEnded:(NSSet*)touches {
    [self forwardTouchesToUnity:touches phase:FFRTouchPhaseEnded];
}

- (void)forwardTouchesToUnity:(NSSet*)touches phase:(FFRTouchPhase)phase {
    if (_receiveDeviceMessageCallback == nil) return;
    
    @autoreleasepool {
        int32_t count = [touches count];
        NSArray *touchesArray = [touches allObjects];
        FFRTouchCStruct *touchesCStructArray = new FFRTouchCStruct[count]();
        
        for (int index = 0; index < count; index++) {
            touchesCStructArray[index] = FFRTouchToCStruct(touchesArray[index]);
            touchesCStructArray[index].phase = phase;
        }
        
        _receiveDeviceMessageCallback(touchesCStructArray, count);
        
        delete [] touchesCStructArray;
    }
}

static FFRTouchC FFRTouchToCStruct(FFRTouch *touch) {
    return (FFRTouchC) {
        .id = touch.identifier,
        .side = touch.side,
        .x = touch.location.x,
        .y = touch.location.y,
        .normx = touch.normalizedLocation.x,
        .normy = touch.normalizedLocation.y,
        .prevx = touch.previousLocation.x,
        .prevy = touch.previousLocation.y
    };
}

@end
