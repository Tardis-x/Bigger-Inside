#import <Foundation/Foundation.h>
#import <UIKit/UIImage.h>

@interface GoodiesUtils : NSObject

+ (NSString *)createNSStringFrom:(const char *)cstring;

+ (NSString *)decodeNSStringFromBase64:(const char *)cstring;

+ (char *)createCStringFrom:(NSString *)string;

+ (NSArray *)createNSArray:(int)count values:(const char **)values;

+ (char *)cStringCopy:(const char *)string;

+ (UIImage *)createImageFromByteArray:(const void *)data dataLength:(const unsigned long)length;

+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic;

+ (NSString *)serializeDictionary:(NSDictionary *)dictionary;

@end
