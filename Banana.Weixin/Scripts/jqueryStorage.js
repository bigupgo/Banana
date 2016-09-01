var storageUtils = {
    setParam: function (name, value) {
        localStorage.setItem(name, value)
    },
    getParam: function (name) {
        return localStorage.getItem(name)
    },
    remove: function (name) {
        localStorage.removeItem(name);
    }
}