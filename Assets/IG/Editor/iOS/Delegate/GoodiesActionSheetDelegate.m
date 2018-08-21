//
// Created by Taras Leskiv on 17/01/2017.
//

#import "GoodiesActionSheetDelegate.h"


@implementation GoodiesActionSheetDelegate

- (void)actionSheet:(UIActionSheet *)actionSheet clickedButtonAtIndex:(NSInteger)buttonIndex {
    NSLog(@"Click %ld", (long) buttonIndex);
    _callbackButtonClicked((long) buttonIndex);
}
@end
