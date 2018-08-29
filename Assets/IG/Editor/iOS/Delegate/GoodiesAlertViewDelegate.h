#import <Foundation/Foundation.h>

@interface GoodiesAlertViewDelegate : NSObject <UIAlertViewDelegate>

@property(nonatomic, copy) void (^callbackButtonClicked)(long index);

@end
