mergeInto(LibraryManager.library, {
    DownloadFile: function (filename, data, size) {
        var blob = new Blob([new Uint8Array(data, size)], { type: 'application/octet-stream' });
        var url = window.URL.createObjectURL(blob);
        var a = document.createElement('a');
        a.href = url;
        a.download = UTF8ToString(filename);
        document.body.appendChild(a);
        a.click();
        document.body.removeChild(a);
        window.URL.revokeObjectURL(url);
    }
});
