//
//  FuffrUnityBridge.h
//  Unity-iPhone
//
//  Created by Andrey Zhukov on 26/08/14.
//
//

#import <Foundation/Foundation.h>

extern "C" {
    struct FFRTouchC;
    typedef struct FFRTouchC FFRTouchCStruct;
    typedef void (*ReceiveDeviceMessageCallback)(FFRTouchCStruct touches[], int32_t length);
}

@interface FFRUnityBridge : NSObject

@property (nonatomic, assign) ReceiveDeviceMessageCallback receiveDeviceMessageCallback;

+ (instancetype)sharedInstance;
- (void)setupWithCallbacksOnUnityGameObjectName:(NSString *)name;

@end
