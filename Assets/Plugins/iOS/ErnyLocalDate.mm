#import <Foundation/Foundation.h>

extern "C" int ErnyGetLocalCalendarDate(void)
{
    @autoreleasepool
    {
        NSCalendar *calendar = [NSCalendar calendarWithIdentifier:NSCalendarIdentifierGregorian];
        calendar.timeZone = [NSTimeZone localTimeZone];
        NSDateComponents *date = [calendar components:(NSCalendarUnitYear | NSCalendarUnitMonth | NSCalendarUnitDay)
                                             fromDate:[NSDate date]];
        return (int)(date.year * 10000 + date.month * 100 + date.day);
    }
}
