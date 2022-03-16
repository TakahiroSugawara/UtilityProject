

var data = {
    items:{
       0:{name:'' , comment:''},
       1:{name:'' , comment:''},
       2:{name:'' , comment:''},
       3:{name:'' , comment:''},
       4:{name:'' , comment:''},
       5:{name:'' , comment:''},
       6:{name:'' , comment:''},
       7:{name:'' , comment:''},
       8:{name:'' , comment:''},
       9:{name:'' , comment:''}
    },

}

var app = new Vue(
    {
        el:'#app', 
        //data file
        data:data,
        methods:{
            sendMessage(index)
            {
                uibuilder.send(this.items[index]);
                console.log(this.items[index]);
            },
                
            /** Called after the Vue app has been created. A good place to put startup code */
            created: function() {
    
                // Example of retrieving data from uibuilder
                this.feVersion = uibuilder.get('version')
    
                /** **REQUIRED** Start uibuilder comms with Node-RED @since v2.0.0-dev3
                 * Pass the namespace and ioPath variables if hosting page is not in the instance root folder
                 * e.g. If you get continual `uibuilderfe:ioSetup: SOCKET CONNECT ERROR` error messages.
                 * e.g. uibuilder.start('/uib', '/uibuilder/vendor/socket.io') // change to use your paths/names
                 * @param {Object=|string=} namespace Optional. Object containing ref to vueApp, Object containing settings, or String IO Namespace override. changes self.ioNamespace from the default.
                 * @param {string=} ioPath Optional. changes self.ioPath from the default
                 * @param {Object=} vueApp Optional. Reference to the VueJS instance. Used for Vue extensions.
                 */
                 uibuilder.start(this) // Single param passing vue app to allow Vue extensions to be used.
                //console.log(this)
            }
    
        },
    
    });
    