//
//  GoodiesAlertViewDelegate.m
//  Unity-iPhone
//
//  Created by Taras Leskiv on 03/09/16.
//
//

#import "GoodiesAlertViewDelegate.h"

@implementation GoodiesAlertViewDelegate

- (void)alertView:(UIAlertView *)alertView clickedButtonAtIndex:(NSInteger)buttonIndex {
    self.callbackButtonClicked(buttonIndex);
}

- (void)alertView:(UIAlertView *)alertView willDismissWithButtonIndex:(NSInteger)buttonIndex {
}

- (void)alertView:(UIAlertView *)alertView didDismissWithButtonIndex:(NSInteger)buttonIndex {
}

@end
