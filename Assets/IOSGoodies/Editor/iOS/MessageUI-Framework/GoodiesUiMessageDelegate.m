//
//  GoodiesUiMessageDelegate.m
//  Unity-iPhone
//
//  Created by Taras Leskiv on 19/09/16.
//
//

#import "GoodiesUiMessageDelegate.h"

@implementation GoodiesUiMessageDelegate
- (void)messageComposeViewController:(MFMessageComposeViewController *)controller didFinishWithResult:(MessageComposeResult)result {
    switch (result) {
        case MessageComposeResultCancelled:
            _callbackCancelled();
            break;
        case MessageComposeResultFailed:
            _callbackFailed();
            break;
        case MessageComposeResultSent:
            _callbackSentSuccessfully();
            break;

        default:
            _callbackCancelled();
            break;
    }

    [controller dismissViewControllerAnimated:YES completion:nil];
}

@end
