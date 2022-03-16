var data = {
    //コメント入力欄
    items:{
       0:{messageKind:"", name:'' , comment:''},
       1:{messageKind:"", name:'' , comment:''},
       2:{messageKind:"", name:'' , comment:''},
       3:{messageKind:"", name:'' , comment:''},
       4:{messageKind:"", name:'' , comment:''}
    },
    //コメント履歴用の空の配列
    Layouts :
    {
        0:{messageKind:"", layout:'A'},
        1:{messageKind:"", layout:'B'},
        2:{messageKind:"", layout:'C'},
        3:{messageKind:"", layout:'D'},
        4:{messageKind:"", layout:'E'}
    },
    Emojis :
    {
        0:{messageKind:"", emoji:'😀'},
        1:{messageKind:"", emoji:'😂'},
        2:{messageKind:"", emoji:'😍'},
        3:{messageKind:"", emoji:'😳'},
        4:{messageKind:"", emoji:'🤔'},
        5:{messageKind:"", emoji:'🧡'},
        6:{messageKind:"", emoji:'👍'},
        7:{messageKind:"", emoji:'👏'},
        8:{messageKind:"", emoji:'🎉'},
        9:{messageKind:"", emoji:'⭐'}
    },

    //NodeRedから受け取る、送信履歴用の空の配列
    manage_message :
    {

    }
}

var app = new Vue(
    {
        el:'#app', 
        data:data,
        methods:
        {
            sendMessage(index)
            {
                console.log(this.items[index]);
                uibuilder.send({
                    messageKind:"0",
                    name:this.items[index].name,
                    comment:this.items[index].comment
                    });
            },
            sort(index)
            {
                this.manage_message = this.manage_message.sort(function(a,b){return(a[index] - b[index]);});;
                console.log(index);
                console.log(this.manage_message);
            },
            sendLayout(index)
            {
                console.log(this.Layouts[index]);
                uibuilder.send({
                    messageKind:"1",
                    layout:this.Layouts[index].layout
                    });
            },
            sendEmoji(index)
            {
                console.log(this.Emojis[index]);
                uibuilder.send({
                    messageKind:"2",
                    emoji:this.Emojis[index].emoji
                    });
            }
        },

        // 変数を加工して渡したい場合に使用する
        computed: {
            // msgs_Rcvd: function() {
            //     var msgRecvd = this.msgRecvd
            //     if (typeof msgRecvd === 'string') return 'Last Message Received = ' + msgRecvd
            //     else return msgRecvd
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

             console.warn(this.feVersion)
        },
    
        /** Called once all Vue component instances have been loaded and the virtual DOM built */
        mounted: function(){

            //console.debug('[indexjs:Vue.mounted] app mounted - setting up uibuilder watchers')
            var app = this  // Reference to `this` in case we need it for more complex functions

            // If msg changes - msg is updated when a standard msg is received from Node-RED over Socket.IO
            uibuilder.onChange('msg', function(msg)
            {
                 app.manage_message = msg.messagelist;
            })
        }
    });
