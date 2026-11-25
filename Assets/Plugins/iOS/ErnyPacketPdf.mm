#import <UIKit/UIKit.h>
#include <math.h>

extern "C" int ErnyRenderPacketPdf(const char *input, const char *output, int index) {
    @autoreleasepool {
        NSURL *url = [NSURL fileURLWithPath:[NSString stringWithUTF8String:input]];
        CGPDFDocumentRef pdf = CGPDFDocumentCreateWithURL((__bridge CFURLRef)url);
        if (!pdf) return 0;

        int count = (int)CGPDFDocumentGetNumberOfPages(pdf);
        CGPDFPageRef page = index >= 0 && index < count ? CGPDFDocumentGetPage(pdf, index + 1) : nullptr;
        if (!page) { CGPDFDocumentRelease(pdf); return 0; }
        CGRect box = CGPDFPageGetBoxRect(page, kCGPDFCropBox);
        int rotation = CGPDFPageGetRotationAngle(page);
        CGFloat width = (rotation % 180 == 0) ? box.size.width : box.size.height;
        CGFloat height = (rotation % 180 == 0) ? box.size.height : box.size.width;
        if (!(width > 0 && height > 0)) { CGPDFDocumentRelease(pdf); return 0; }
        CGFloat scale = 1600 / MAX(width, height);
        CGSize size = CGSizeMake(MAX(1, round(width * scale)), MAX(1, round(height * scale)));

        UIGraphicsImageRendererFormat *format = [UIGraphicsImageRendererFormat defaultFormat];
        format.scale = 1;
        format.opaque = YES;
        UIGraphicsImageRenderer *renderer = [[UIGraphicsImageRenderer alloc] initWithSize:size format:format];
        NSData *png = [renderer PNGDataWithActions:^(UIGraphicsImageRendererContext *target) {
            CGRect bounds = CGRectMake(0, 0, size.width, size.height);
            CGContextRef context = target.CGContext;
            CGContextSetFillColorWithColor(context, UIColor.whiteColor.CGColor);
            CGContextFillRect(context, bounds);
            CGContextTranslateCTM(context, 0, size.height);
            CGContextScaleCTM(context, 1, -1);
            CGContextConcatCTM(context, CGPDFPageGetDrawingTransform(page, kCGPDFCropBox, bounds, 0, true));
            CGContextDrawPDFPage(context, page);
        }];
        BOOL saved = [png writeToFile:[NSString stringWithUTF8String:output] atomically:YES];
        CGPDFDocumentRelease(pdf);
        return saved ? count : 0;
    }
}
