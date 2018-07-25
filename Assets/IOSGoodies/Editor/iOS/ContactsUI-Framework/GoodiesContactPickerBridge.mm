#import <ContactsUI/ContactsUI.h>
#import "GoodiesContactPickerDelegate.h"
#import "BridgeGoodisFunctionDefs.h"
#import "GoodiesUtils.h"

#pragma clang diagnostic push
#pragma ide diagnostic ignored "OCUnusedGlobalDeclarationInspection"
extern "C" {

GoodiesContactPickerDelegate *goodiesContactPickerDelegate;

    void _showContactPicker(ActionStringCallbackDelegate successCallback, ActionStringCallbackDelegate cancelCallback,
                            void *successPtr, void *errPtr) {
    CNContactPickerViewController *picker = [[CNContactPickerViewController alloc] init];

    goodiesContactPickerDelegate = [GoodiesContactPickerDelegate new];
    goodiesContactPickerDelegate.pickingCancelled = ^{
        cancelCallback(errPtr, [GoodiesUtils createCStringFrom:@"Cancelled"]);
    };
    goodiesContactPickerDelegate.pickingSuccess = ^(NSString *serializedContact) {
        successCallback(successPtr, serializedContact.UTF8String);
    };
    picker.delegate = goodiesContactPickerDelegate;
    [UnityGetGLViewController() presentViewController:picker animated:YES completion:nil];
}
}

#pragma clang diagnostic pop
