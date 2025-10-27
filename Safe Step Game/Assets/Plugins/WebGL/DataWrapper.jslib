mergeInto(LibraryManager.library, {
  HelloWorld: function() {
    console.log("Hello World from JS!");
  },

  AccessLocalStorage: function() {
    console.log("Accessing local storage...");
  },

  ReadLocalStorage: function (keyPtr) {
    var key = UTF8ToString(keyPtr);
    var value = localStorage.getItem(key);
    if (value == null) value = "";
    var lengthBytes = lengthBytesUTF8(value) + 1;
    var stringOnWasmHeap = _malloc(lengthBytes);
    stringToUTF8(value, stringOnWasmHeap, lengthBytes);
    return stringOnWasmHeap;
  },

  WriteLocalStorage: function (keyPtr, valuePtr) {
    var key = UTF8ToString(keyPtr);
    var value = UTF8ToString(valuePtr);
    localStorage.setItem(key, value);
  },

  ClearLocalStorage: function() {
    localStorage.clear();
  }
});
