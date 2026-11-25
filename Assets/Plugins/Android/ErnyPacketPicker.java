package com.blingar;

import android.app.Activity;
import android.app.Fragment;
import android.content.Intent;
import android.database.Cursor;
import android.net.Uri;
import android.provider.OpenableColumns;
import com.unity3d.player.UnityPlayer;
import java.io.File;
import java.io.FileOutputStream;
import java.io.InputStream;
import java.util.UUID;

public final class ErnyPacketPicker extends Fragment {
    private static final String TAG = "ErnyPacketPicker";
    private static final int PICK = 4517;
    private static final int LIMIT = 25 * 1024 * 1024;

    public static void open(Activity activity) {
        activity.runOnUiThread(() -> {
            if (activity.getFragmentManager().findFragmentByTag(TAG) != null) return;
            ErnyPacketPicker picker = new ErnyPacketPicker();
            activity.getFragmentManager().beginTransaction().add(picker, TAG).commitNowAllowingStateLoss();
            Intent intent = new Intent(Intent.ACTION_OPEN_DOCUMENT);
            intent.setType("*/*");
            intent.addCategory(Intent.CATEGORY_OPENABLE);
            intent.putExtra(Intent.EXTRA_MIME_TYPES, new String[]{"application/pdf","image/jpeg","image/png"});
            try { picker.startActivityForResult(intent, PICK); }
            catch (Exception e) {
                picker.detach();
                send("PickFailed", "Couldn't open the file picker.");
            }
        });
    }

    @Override
    public void onActivityResult(int request, int result, Intent data) {
        if (request != PICK) return;
        Activity activity = getActivity();
        Uri uri = data == null ? null : data.getData();
        detach();
        if (result != Activity.RESULT_OK || uri == null || activity == null) { send("Picked",""); return; }
        new Thread(() -> copy(activity, uri)).start();
    }

    private void detach() {
        getFragmentManager().beginTransaction().remove(this).commitAllowingStateLoss();
    }

    private static void copy(Activity activity, Uri uri) {
        File file = null, dir = null;
        try {
            String name = null;
            try (Cursor cursor = activity.getContentResolver().query(uri, new String[]{OpenableColumns.DISPLAY_NAME}, null, null, null)) {
                if (cursor != null && cursor.moveToFirst()) name = cursor.getString(0);
            }
            if (name == null || !name.matches("(?i).+\\.(pdf|png|jpe?g)")) throw new IllegalArgumentException();
            name = name.replace('/', '_').replace('\\', '_');
            dir = new File(activity.getCacheDir(), "erny-packet-" + UUID.randomUUID());
            if (!dir.mkdir()) throw new IllegalStateException();
            file = new File(dir, name);
            try (InputStream input = activity.getContentResolver().openInputStream(uri);
                 FileOutputStream output = new FileOutputStream(file)) {
                byte[] buffer = new byte[8192];
                int count, total = 0;
                while ((count = input.read(buffer)) != -1) {
                    total += count;
                    if (total > LIMIT) throw new IllegalArgumentException();
                    output.write(buffer, 0, count);
                }
                if (total == 0) throw new IllegalArgumentException();
            }
            send("Picked", file.getAbsolutePath());
        } catch (Exception e) {
            if (file != null) file.delete();
            if (dir != null) dir.delete();
            send("PickFailed", "Couldn't read the file. Choose a PDF, JPG or PNG up to 25 MB.");
        }
    }

    private static void send(String method, String value) {
        UnityPlayer.UnitySendMessage("Erny Packet Picker", method, value);
    }
}
