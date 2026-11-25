package com.blingar;

import android.graphics.Bitmap;
import android.graphics.Color;
import android.graphics.pdf.PdfRenderer;
import android.os.ParcelFileDescriptor;
import java.io.File;
import java.io.FileOutputStream;

public final class ErnyPacketPdf {
    public static int render(String input, String output, int index) {
        try (ParcelFileDescriptor file = ParcelFileDescriptor.open(new File(input), ParcelFileDescriptor.MODE_READ_ONLY);
             PdfRenderer renderer = new PdfRenderer(file)) {
            if (index < 0 || index >= renderer.getPageCount()) return 0;
            try (PdfRenderer.Page page = renderer.openPage(index);
                 FileOutputStream png = new FileOutputStream(output)) {
                float scale = 1600f / Math.max(page.getWidth(), page.getHeight());
                Bitmap image = Bitmap.createBitmap(Math.max(1, Math.round(page.getWidth() * scale)),
                    Math.max(1, Math.round(page.getHeight() * scale)), Bitmap.Config.ARGB_8888);
                try {
                    image.eraseColor(Color.WHITE);
                    page.render(image, null, null, PdfRenderer.Page.RENDER_MODE_FOR_DISPLAY);
                    return image.compress(Bitmap.CompressFormat.PNG, 100, png) ? renderer.getPageCount() : 0;
                } finally { image.recycle(); }
            }
        } catch (Exception ignored) { return 0; }
    }
}
