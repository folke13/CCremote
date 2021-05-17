// Create WebSocket connection.
const socket = new WebSocket('localhost:12345');

// Listen for messages
socket.addEventListener('message', function (event) {
    console.log('Message from server ', event.data);
    updateData()
});

function updateData(){
  document.getElementById("testValue").innerHTML = "I'm a changed string, I promise!";
}
