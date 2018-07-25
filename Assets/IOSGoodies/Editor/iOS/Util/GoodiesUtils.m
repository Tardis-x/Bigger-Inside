#import "GoodiesUtils.h"

@implementation GoodiesUtils

// Converts C style string to NSString
+ (NSString *)createNSStringFrom:(const char *)cstring {
    return [NSString stringWithUTF8String:(cstring ?: "")];
}

+ (NSString *)decodeNSStringFromBase64:(const char *)cstring {
    NSString *base64 = [self createNSStringFrom:cstring];
    NSData *decodedData = [[NSData alloc] initWithBase64EncodedString:base64 options:0];
    return [[NSString alloc] initWithData:decodedData encoding:NSUTF8StringEncoding];
}

+ (NSArray *)createNSArray:(int)count values:(const char **)values {
    if (count == 0) {
        return nil;
    }

    NSMutableArray *mutableArray = [NSMutableArray array];

    for (NSUInteger i = 0; i < count; i++) {
        mutableArray[i] = [self createNSStringFrom:values[i]];
    }

    return mutableArray;
}

+ (char *)cStringCopy:(const char *)string {
    char *res = (char *) malloc(strlen(string) + 1);
    strcpy(res, string);
    return res;
}

+ (char *)createCStringFrom:(NSString *)string {
    if (!string) {
        string = @"";
    }
    return [self cStringCopy:[string UTF8String]];
}

+ (UIImage *)createImageFromByteArray:(const void *)data dataLength:(const unsigned long)length {
    NSData *imageData = [[NSData alloc] initWithBytes:data length:length];
    UIImage *image = [UIImage imageWithData:imageData];
    return image;
}

+ (NSDictionary *)deserializeDictionary:(NSString *)jsonDic
{
    NSError *e = nil;
    NSDictionary *dictionary = [NSJSONSerialization JSONObjectWithData:[jsonDic dataUsingEncoding:NSUTF8StringEncoding] options:NSJSONReadingMutableContainers error:&e];
    if (dictionary != nil)
    {
        NSMutableDictionary *prunedDict = [NSMutableDictionary dictionary];
        [dictionary enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
            if (![obj isKindOfClass:[NSNull class]]) {
                prunedDict[key] = obj;
            }
        }];
        return prunedDict;
    }
    return dictionary;
}

+ (NSString *)serializeDictionary:(NSDictionary *)dictionary
{
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:dictionary
                                                       options:nil
                                                         error:&error];
    return [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
}
@end
