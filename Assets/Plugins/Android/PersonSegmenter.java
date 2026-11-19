package com.blingar;

import android.graphics.Bitmap;
import android.graphics.Canvas;
import android.graphics.Matrix;
import android.graphics.Paint;

import com.google.mlkit.vision.common.InputImage;
import com.google.mlkit.vision.segmentation.Segmentation;
import com.google.mlkit.vision.segmentation.SegmentationMask;
import com.google.mlkit.vision.segmentation.Segmenter;
import com.google.mlkit.vision.segmentation.selfie.SelfieSegmenterOptions;

import java.nio.ByteBuffer;
import java.nio.ByteOrder;

public final class PersonSegmenter {
    private final Segmenter segmenter = Segmentation.getClient(new SelfieSegmenterOptions.Builder()
            .setDetectorMode(SelfieSegmenterOptions.STREAM_MODE)
            .build());
    private final Matrix rotation = new Matrix();
    private final Canvas canvas = new Canvas();
    private final Paint paint = new Paint(Paint.FILTER_BITMAP_FLAG);

    private Bitmap source;
    private Bitmap upright;
    private byte[] confidence;
    private Result result;
    private boolean processing;
    private boolean closed;

    public synchronized void submit(byte[] rgba, int width, int height, int rotationDegrees) {
        if (closed || processing || result != null) {
            throw new IllegalStateException("The selfie segmenter is not ready for another frame.");
        }

        processing = true;
        try {
            source = resize(source, width, height);
            source.copyPixelsFromBuffer(ByteBuffer.wrap(rgba));

            Bitmap input = source;
            if (rotationDegrees != 0) {
                boolean swapAxes = rotationDegrees == 90 || rotationDegrees == 270;
                upright = resize(upright, swapAxes ? height : width, swapAxes ? width : height);
                rotation.setRotate(rotationDegrees);
                rotation.postTranslate(rotationDegrees == 90 ? height : rotationDegrees == 180 ? width : 0,
                        rotationDegrees == 270 ? width : rotationDegrees == 180 ? height : 0);
                canvas.setBitmap(upright);
                canvas.drawBitmap(source, rotation, paint);
                canvas.setBitmap(null);
                input = upright;
            }

            // Rotate the bitmap explicitly so the returned mask has a known coordinate space.
            segmenter.process(InputImage.fromBitmap(input, 0)).addOnCompleteListener(task -> {
                if (task.isSuccessful()) {
                    complete(task.getResult());
                } else {
                    Exception exception = task.getException();
                    fail(exception != null ? exception.toString() : "Segmentation was cancelled.");
                }
            });
        } catch (RuntimeException exception) {
            fail(exception.toString());
        }
    }

    public synchronized Object takeResult() {
        Result completed = result;
        result = null;
        return completed;
    }

    public synchronized void close() {
        if (closed) {
            return;
        }

        closed = true;
        result = null;
        segmenter.close();
        if (!processing) {
            recycleBitmaps();
        }
    }

    private synchronized void complete(SegmentationMask mask) {
        if (closed) {
            processing = false;
            recycleBitmaps();
            return;
        }

        try {
            int width = mask.getWidth();
            int height = mask.getHeight();
            int size = width * height;
            if (confidence == null || confidence.length != size) {
                confidence = new byte[size];
            }

            ByteBuffer values = mask.getBuffer().order(ByteOrder.nativeOrder());
            values.rewind();
            for (int i = 0; i < size; ++i) {
                confidence[i] = (byte) Math.round(Math.max(0, Math.min(1, values.getFloat())) * 255);
            }
            result = new Result(confidence, width, height, null);
            processing = false;
        } catch (RuntimeException exception) {
            fail(exception.toString());
        }
    }

    private synchronized void fail(String error) {
        processing = false;
        if (closed) {
            recycleBitmaps();
        } else {
            result = new Result(null, 0, 0, error);
        }
    }

    private static Bitmap resize(Bitmap bitmap, int width, int height) {
        if (bitmap != null && bitmap.getWidth() == width && bitmap.getHeight() == height) {
            return bitmap;
        }
        if (bitmap != null) {
            bitmap.recycle();
        }
        return Bitmap.createBitmap(width, height, Bitmap.Config.ARGB_8888);
    }

    private void recycleBitmaps() {
        if (source != null) {
            source.recycle();
            source = null;
        }
        if (upright != null) {
            upright.recycle();
            upright = null;
        }
        confidence = null;
    }

    public static final class Result {
        public final byte[] confidence;
        public final int width;
        public final int height;
        public final String error;

        private Result(byte[] confidence, int width, int height, String error) {
            this.confidence = confidence;
            this.width = width;
            this.height = height;
            this.error = error;
        }
    }
}
