#import <Foundation/Foundation.h>
#import <MessageUI/MessageUI.h>

@interface GoodiesUiMessageDelegate : NSObject <MFMessageComposeViewControllerDelegate>

@property(nonatomic, copy) void (^callbackSentSuccessfully)();

@property(nonatomic, copy) void (^callbackCancelled)();

@property(nonatomic, copy) void (^callbackFailed)();

@end
