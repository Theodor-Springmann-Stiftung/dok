function createDictionary(o) {
    if (!o) return;
    var res = {};
    for (var i =  0; i < o.length; i++) {
        res[o[i].id] = o[i];
    }
    return res;
}

export default createDictionary;