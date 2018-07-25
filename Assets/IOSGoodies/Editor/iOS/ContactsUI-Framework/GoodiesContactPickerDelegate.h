#import <Foundation/Foundation.h>
#import <ContactsUI/ContactsUI.h>

@interface GoodiesContactPickerDelegate : NSObject <CNContactPickerDelegate>

@property(nonatomic, copy) void (^pickingCancelled)();

@property(nonatomic, copy) void (^pickingSuccess)(NSString *serializedContact);

@end
