//
// Created by Taras Leskiv on 17/01/2017.
//

#import <Foundation/Foundation.h>


@interface GoodiesActionSheetDelegate : NSObject <UIActionSheetDelegate>

@property(nonatomic, copy) void (^callbackButtonClicked)(long index);

@end