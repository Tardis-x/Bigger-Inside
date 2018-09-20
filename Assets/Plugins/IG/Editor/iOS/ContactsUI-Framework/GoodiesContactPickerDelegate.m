#import "GoodiesContactPickerDelegate.h"
#import "GoodiesUtils.h"

@implementation GoodiesContactPickerDelegate {

}

- (void)contactPicker:(CNContactPickerViewController *)picker didSelectContact:(CNContact *)contact {
    NSString *serializeContact = [self serializeContact:contact];
    _pickingSuccess(serializeContact);
}

- (void)contactPickerDidCancel:(CNContactPickerViewController *)picker {
    _pickingCancelled();
}

- (NSString *)serializeContact:(CNContact *)contact {
    NSMutableDictionary *rootDic = [NSMutableDictionary new];

    [self addValuesFromContact:contact rootDic:rootDic];
    [self addPhoneNumbersFromContact:contact rootDic:rootDic];
    [self addEMailsFromContact:contact rootDic:rootDic];
    [self addPostalAddressesFromContact:contact rootDic:rootDic];
    [self addUrlAddressesFromContact:contact rootDic:rootDic];
    [self addContactRelationsFromContact:contact rootDic:rootDic];
    [self addSocialProfilesFromContact:contact rootDic:rootDic];
    [self addInstantMessageAddressesFromContact:contact rootDic:rootDic];
    [self addDatesFromContact:contact rootDic:rootDic];

    return [GoodiesUtils serializeDictionary:rootDic];
}

- (void)addDatesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *datesArr = [NSMutableArray new];
    for (CNLabeledValue<NSDateComponents *> *date in contact.dates) {
        NSMutableDictionary *datesDic = [self serializeDate:date.value];
        datesDic[@"label"] = date.label;
        [datesArr addObject:datesDic];
    }
    rootDic[@"dates"] = datesArr;
}

- (void)addValuesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    [rootDic setValue:contact.identifier forKey:@"identifier"];
    [rootDic setValue:contact.namePrefix forKey:@"namePrefix"];
    [rootDic setValue:contact.givenName forKey:@"givenName"];
    [rootDic setValue:contact.middleName forKey:@"middleName"];
    [rootDic setValue:contact.familyName forKey:@"familyName"];
    [rootDic setValue:contact.previousFamilyName forKey:@"previousFamilyName"];
    [rootDic setValue:contact.nameSuffix forKey:@"nameSuffix"];
    [rootDic setValue:contact.nickname forKey:@"nickname"];

    [rootDic setValue:contact.organizationName forKey:@"organizationName"];
    [rootDic setValue:contact.departmentName forKey:@"departmentName"];
    [rootDic setValue:contact.jobTitle forKey:@"jobTitle"];

    [rootDic setValue:contact.phoneticGivenName forKey:@"phoneticGivenName"];
    [rootDic setValue:contact.phoneticMiddleName forKey:@"phoneticMiddleName"];
    [rootDic setValue:contact.phoneticFamilyName forKey:@"phoneticFamilyName"];
    if (@available(iOS 10.0, *)) {
        [rootDic setValue:contact.phoneticOrganizationName forKey:@"phoneticOrganizationName"];
    }
    rootDic[@"birthday"] = [self serializeDate:contact.birthday];

    [rootDic setValue:contact.note forKey:@"note"];
}

- (void)addPhoneNumbersFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *phoneNumbers = [NSMutableArray new];
    for (CNLabeledValue<CNPhoneNumber *> *phoneNumber in contact.phoneNumbers) {
        NSMutableDictionary *phonesDic = [self serializeLabeledValue:phoneNumber.label value:phoneNumber.value.stringValue];
        [phoneNumbers addObject:phonesDic];
    }
    rootDic[@"phoneNumbers"] = phoneNumbers;
}

- (void)addEMailsFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *emails = [NSMutableArray new];
    for (CNLabeledValue<NSString *> *email in contact.emailAddresses) {
        NSMutableDictionary *emailsDic = [self serializeLabeledValue:email.label value:email.value];
        [emails addObject:emailsDic];
    }
    rootDic[@"emails"] = emails;
}

- (void)addPostalAddressesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *postalAddresses = [NSMutableArray new];
    for (CNLabeledValue<CNPostalAddress *> *postalAddress in contact.postalAddresses) {
        NSMutableDictionary *addrDic = [NSMutableDictionary new];

        addrDic[@"label"] = postalAddress.label;
        addrDic[@"street"] = postalAddress.value.street;
        addrDic[@"subLocality"] = postalAddress.value.subLocality;
        addrDic[@"city"] = postalAddress.value.city;
        addrDic[@"subAdministrativeArea"] = postalAddress.value.subAdministrativeArea;
        addrDic[@"state"] = postalAddress.value.state;
        addrDic[@"postalCode"] = postalAddress.value.postalCode;
        addrDic[@"country"] = postalAddress.value.country;
        addrDic[@"ISOCountryCode"] = postalAddress.value.ISOCountryCode;

        [postalAddresses addObject:addrDic];
    }
    rootDic[@"postalAddresses"] = postalAddresses;
}

- (void)addUrlAddressesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *urlAddresses = [NSMutableArray new];
    for (CNLabeledValue<NSString *> *urlAddress in contact.urlAddresses) {
        NSMutableDictionary *urlAddrDic = [self serializeLabeledValue:urlAddress.label value:urlAddress.value];
        [urlAddresses addObject:urlAddrDic];
    }
    rootDic[@"urlAddresses"] = urlAddresses;
}

- (void)addContactRelationsFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *contactRelations = [NSMutableArray new];
    for (CNLabeledValue<CNContactRelation *> *contactRelation in contact.contactRelations) {
        NSMutableDictionary *urlAddrDic = [self serializeLabeledValue:contactRelation.label value:contactRelation.value.name];
        [contactRelations addObject:urlAddrDic];
    }
    rootDic[@"contactRelations"] = contactRelations;
}

- (void)addInstantMessageAddressesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *instantMessageAddresses = [NSMutableArray new];
    for (CNLabeledValue<CNInstantMessageAddress *> *imAddress in contact.instantMessageAddresses) {
        NSMutableDictionary *imAddressDic = [NSMutableDictionary new];

        imAddressDic[@"label"] = imAddress.label;
        imAddressDic[@"service"] = imAddress.value.service;
        imAddressDic[@"username"] = imAddress.value.username;

        [instantMessageAddresses addObject:imAddressDic];
    }
    rootDic[@"instantMessageAddresses"] = instantMessageAddresses;
}

- (void)addSocialProfilesFromContact:(CNContact *)contact rootDic:(NSMutableDictionary *)rootDic {
    NSMutableArray *socialProfiles = [NSMutableArray new];
    for (CNLabeledValue<CNSocialProfile *> *socialProfile in contact.socialProfiles) {
        NSMutableDictionary *socialProfDic = [NSMutableDictionary new];

        socialProfDic[@"label"] = socialProfile.label;
        socialProfDic[@"urlString"] = socialProfile.value.urlString;
        socialProfDic[@"username"] = socialProfile.value.username;
        socialProfDic[@"userIdentifier"] = socialProfile.value.userIdentifier;
        socialProfDic[@"service"] = socialProfile.value.service;

        [socialProfiles addObject:socialProfDic];
    }
    rootDic[@"socialProfiles"] = socialProfiles;
}

- (NSMutableDictionary *)serializeLabeledValue:(NSString *)label value:(NSString *)value {
    NSMutableDictionary *dic = [NSMutableDictionary new];
    dic[@"label"] = label;
    dic[@"value"] = value;
    return dic;
}

- (NSMutableDictionary *)serializeDate:(NSDateComponents *)date {
    NSMutableDictionary *dateDic = [NSMutableDictionary new];
    dateDic[@"year"] = @(date.year);
    dateDic[@"month"] = @(date.month);
    dateDic[@"day"] = @(date.day);
    return dateDic;
}

@end
