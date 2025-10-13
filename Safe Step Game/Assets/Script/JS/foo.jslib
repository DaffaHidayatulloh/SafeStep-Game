mergeInto(LibraryManager.library, {
    HelloWorld: function() {
        window.alert("Hello, world!");
    },

    AccessLocalStorage: function(key, value) {
        if (value) {
            localStorage.setItem(key, value);
            return "done";
        } else {
            return (localStorage.getItem(key) || "");
        }
    },

    ClearLocalStorage: function() {
        localStorage.clear();
    },

    ReadLocalStorage: function(key) {
        return (localStorage.getItem(key) || "");
    },

    WriteLocalStorage: function(key, value) {
        localStorage.setItem(key, value);
    },
} );