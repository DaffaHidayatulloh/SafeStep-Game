mergeInto(LibraryManager.library, {
    DownloadFile: function (array, length, fileNamePtr) {
        // Ambil nama file dari pointer string Unity
        var fileName = UTF8ToString(fileNamePtr);

        // Ambil data byte array dari Unity memory
        var data = new Uint8Array(length);
        data.set(HEAPU8.subarray(array, array + length));

        // Buat Blob (file binary)
        var blob = new Blob([data], { type: 'image/png' });

        // Buat link download
        var url = URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        URL.revokeObjectURL(url);
    }
});
