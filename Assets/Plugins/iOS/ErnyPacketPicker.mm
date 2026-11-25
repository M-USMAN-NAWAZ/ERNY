#import <UIKit/UIKit.h>
#import "UnityInterface.h"

@interface ErnyPacketPicker : NSObject <UIDocumentPickerDelegate>
@end

static ErnyPacketPicker *packetPicker;

@implementation ErnyPacketPicker
- (void)documentPickerWasCancelled:(UIDocumentPickerViewController *)controller {
    UnitySendMessage("Erny Packet Picker", "Picked", "");
    packetPicker = nil;
}

- (void)documentPicker:(UIDocumentPickerViewController *)controller didPickDocumentsAtURLs:(NSArray<NSURL *> *)urls {
    NSURL *source = urls.firstObject;
    BOOL access = [source startAccessingSecurityScopedResource];
    id size = nil;
    [source getResourceValue:&size forKey:NSURLFileSizeKey error:nullptr];
    NSString *folder = [NSTemporaryDirectory() stringByAppendingPathComponent:
        [@"erny-packet-" stringByAppendingString:NSUUID.UUID.UUIDString]];
    NSString *path = source ? [folder stringByAppendingPathComponent:source.lastPathComponent] : nil;
    NSFileManager *files = NSFileManager.defaultManager;
    long long fileSize = [size isKindOfClass:[NSNumber class]] ? [(NSNumber *)size longLongValue] : 0;
    BOOL copied = fileSize > 0 && fileSize <= 25LL * 1024 * 1024 &&
        [files createDirectoryAtPath:folder withIntermediateDirectories:NO attributes:nil error:nullptr] &&
        [files copyItemAtURL:source toURL:[NSURL fileURLWithPath:path] error:nullptr];
    if (access) [source stopAccessingSecurityScopedResource];
    if (copied) UnitySendMessage("Erny Packet Picker", "Picked", path.UTF8String);
    else {
        [files removeItemAtPath:folder error:nullptr];
        UnitySendMessage("Erny Packet Picker", "PickFailed", "Couldn't read the file. Choose a PDF, JPG or PNG up to 25 MB.");
    }
    packetPicker = nil;
}
@end

extern "C" void ErnyPickPacketFile() {
    dispatch_async(dispatch_get_main_queue(), ^{
        UIViewController *presenter = UnityGetGLViewController();
        if (!presenter || presenter.presentedViewController) {
            UnitySendMessage("Erny Packet Picker", "PickFailed", "Couldn't open the file picker.");
            return;
        }
        packetPicker = [ErnyPacketPicker new];
        UIDocumentPickerViewController *picker = [[UIDocumentPickerViewController alloc]
            initWithDocumentTypes:@[@"com.adobe.pdf", @"public.jpeg", @"public.png"] inMode:UIDocumentPickerModeImport];
        picker.delegate = packetPicker;
        [presenter presentViewController:picker animated:YES completion:nil];
    });
}
