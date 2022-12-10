// https://github.com/swagger-api/swagger-ui/issues/3876#issuecomment-650697211
const AdvancedFilterPlugin = function (system) {
    return {
        fn: {
            opsFilter: function (taggedOps, phrase) {
                phrase = phrase ? phrase.toLowerCase() : '';
                var normalTaggedOps = JSON.parse(JSON.stringify(taggedOps));
                for (tagObj in normalTaggedOps) {
                    var operations = normalTaggedOps[tagObj].operations;
                    var i = operations.length;
                    while (i--) {
                        var operation = operations[i].operation;
                        if ((!operations[i].path || operations[i].path.toLowerCase().indexOf(phrase) === -1)
                            && (!operation.summary || operation.summary.toLowerCase().indexOf(phrase) === -1)
                            && (!operation.description || operation.description.toLowerCase().indexOf(phrase) === -1)
                        ) {
                            operations.splice(i, 1);
                        }
                    }
                    if (operations.length == 0) {
                        delete normalTaggedOps[tagObj];
                    } else {
                        normalTaggedOps[tagObj].operations = operations;
                    }
                }

                return system.Im.fromJS(normalTaggedOps);
            }
        }
    };
};

if (!configObject.plugins)
    configObject.plugins = [];
configObject.plugins.push(AdvancedFilterPlugin);