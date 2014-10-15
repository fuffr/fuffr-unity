//
//  FuffrUnityBridge.m
//  Unity-iPhone
//
//  Created by Andrey Zhukov on 26/08/14.
//
//

#import "FFRUnityBridge.h"
#import "FuffrLib/FFRTouchManager.h"

#define GetStringParam( _x_ ) ( _x_ != NULL ) ? [NSString stringWithUTF8String:_x_] : [NSString stringWithUTF8String:""]

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
    
    void _fuffrSetupWithCallbacksOnUnityGameObjectName(const char *callbackName) {
        [[FFRUnityBridge sharedInstance] setupWithCallbacksOnUnityGameObjectName:GetStringParam(callbackName)];
    }
    
    void _RegisterRecieveDeviceMessageCallback(ReceiveDeviceMessageCallback callback) {
        [FFRUnityBridge sharedInstance].receiveDeviceMessageCallback = callback;
    }
    
    void _StartTestTimer() {
//        dispatch_source_t timer = dispatch_source_create(DISPATCH_SOURCE_TYPE_TIMER, 0, 0, dispatch_get_main_queue());
//        dispatch_source_set_timer(timer, DISPATCH_TIME_NOW, 5 * NSEC_PER_SEC, 60 * NSEC_PER_SEC);
//        
//        FuffrUnityBridge *sharedInstance = [FuffrUnityBridge sharedInstance];
//        
//        dispatch_source_set_event_handler(timer, ^{
//            [sharedInstance testTick];
//        });
//        
//        sharedInstance.timer = timer;
//        dispatch_resume(sharedInstance.timer);
    }
    
    void _StopTestTimer() {
//        FuffrUnityBridge *sharedInstance = [FuffrUnityBridge sharedInstance];
//        if (sharedInstance.timer != NULL) {
//            dispatch_source_cancel(sharedInstance.timer);
//        }
    }
}

static NSString *const kFuffrUnityBridgeFuffrConnected = @"OnFuffrConnected";
static NSString *const kFuffrUnityBridgeFuffrDisconnected = @"OnFuffrDisconnected";
static NSString *const kFuffrUnityBridgeFuffrTouchBegan = @"OnFuffrTouchBegan";
static NSString *const kFuffrUnityBridgeFuffrTouchMoved = @"OnFuffrTouchMoved";
static NSString *const kFuffrUnityBridgeFuffrTouchEnded = @"OnFuffrTouchEnded";

@interface FFRUnityBridge()

@property (nonatomic, copy) NSString *callbackUnityGameObjectName;
@property (nonatomic, assign) dispatch_source_t timer;

//- (void)testTick;

@end

@implementation FFRUnityBridge

+ (instancetype)sharedInstance {
    static dispatch_once_t pred = 0;
    static id instance = nil;
    
    dispatch_once(&pred, ^{
        instance = [[self alloc] init];
    });
    return instance;
}

- (instancetype)init {
    self = [super init];
    if (self) {
        [self setupUsingSides:(FFRSide)(FFRSideLeft | FFRSideRight | FFRSideBottom | FFRSideTop)];        
    }
    return self;
}

- (void)setupWithCallbacksOnUnityGameObjectName:(NSString *)name {
    NSAssert([name length] > 0, @"Unity GameObject name cannot be empty or nil.");
    
    self.callbackUnityGameObjectName = name;
    [self setupUsingSides:(FFRSide)(FFRSideLeft | FFRSideRight | FFRSideBottom | FFRSideTop)];
}

- (void)setupUsingSides:(FFRSide)sides {
	NSLog(@"Scanning for Fuffr...");
    
	// Get a reference to the touch manager.
	FFRTouchManager* manager = [FFRTouchManager sharedManager];
    
	[manager
     onFuffrConnected:
     ^{
         NSLog(@"Fuffr Connected");
         UnitySendMessage(self.callbackUnityGameObjectName.UTF8String, kFuffrUnityBridgeFuffrConnected.UTF8String, "");
         
         [manager useSensorService:^{
              // Sensor is available, set active sides.
              [[FFRTouchManager sharedManager]
               enableSides: (FFRSide) (FFRSideLeft | FFRSideRight)
               touchesPerSide: @1];
          }];
     }
     onFuffrDisconnected:^{
         NSLog(@"Fuffr Disconnected");
         UnitySendMessage(self.callbackUnityGameObjectName.UTF8String, kFuffrUnityBridgeFuffrDisconnected.UTF8String, "");
     }];
    
	// Register methods for touch events. Here the side constants are
	// bit-or:ed to capture touches on all four sides.
	[manager addTouchObserver:self
                   touchBegan:@selector(touchesBegan:)
                   touchMoved:@selector(touchesMoved:)
                   touchEnded:@selector(touchesEnded:)
                        sides:sides];
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
    if (_receiveDeviceMessageCallback != NULL) {
        int32_t count = [touches count];
        NSArray *touchesArray = [touches allObjects];
        FFRTouchCStruct *touchesCArray = new FFRTouchCStruct[count]();
        
        for (int index = 0; index < count; index++) {
            touchesCArray[index] = CovertToCStruct(touchesArray[index]);
            touchesCArray[index].phase = phase;            
        }
        
        _receiveDeviceMessageCallback(touchesCArray, count);
        
        delete [] touchesCArray;
    }
}

FFRTouchC CovertToCStruct(FFRTouch *touch) {
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

//- (void)testTick {
//    FFRTouchCStruct touch = {
//        .id = 27,
//        .side = FFRSideNotSet,
//        .phase = FFRTouchPhaseBegan,
//        .x = 13,
//        .y = 14,
//        .normx = 33,
//        .normy = 34,
//        .prevx = 23,
//        .prevy = 24
//    };
//    
//    int32_t count = 3;
//    FFRTouchCStruct *touches = new FFRTouchCStruct[count]();
//    
//    touches[0] = touch;
//    touches[1] = touch;
//    touches[2] = touch;
//
//    if (_receiveDeviceMessageCallback != NULL) {
//        _receiveDeviceMessageCallback(touches, count);
//    }
//    
//    delete [] touches;
//}

@end
