window.addEventListener("load",(function(){websocket=new WebSocket("ws://localhost:8080/websocket"),websocket.onclose=function(o){console.log("closing"),websocket.close(),document.location.reload()}}),!1);