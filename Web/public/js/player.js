var playlist;

// View add music modal when user click add music button
document.getElementById('plus-btn').onclick = function () {
    document.getElementById('modal-bg').style.display = 'block';
    document.getElementById('modal-container').style.display = 'block';
}

// Close add muic modal when user click modal background
 function closeModal () {
    document.getElementById('modal-container').style.display = 'none';
    document.getElementById('modal-bg').style.display = 'none';
    document.querySelector("iframe").contentWindow.postMessage("Reset","*");
}

document.getElementById('modal-bg').onclick = closeModal;
// document.getElementById('cancel').onclick = closeModal;

// 
window.onload = function(){
    // playlist = getPlayList();
    
}

// function getPlayList(){
//     var xhr = new XMLHttpRequest();
//     xhr.open('GET',`localhost:3000/getPlayList/${userName}/${playlistName}`);
//     xhr.onreadystatechange = function(){
//         if(xhr.status == 200 && xhr.readyState==4){
//             var data = xhr.responseText;
//             return data;
//         }
//     }
// }

// This function excute when change file input
// It show waiting list music upload file
function fileReader(input) {
    var length = input.files.length;
    var fileList = document.getElementById('file-list');
    fileList.innerHTML = "";
    for (i = 0; i < length; i++) {
        let li = document.createElement('li');
        li.innerHTML = input.files[i].name + " &nbsp;&nbsp; |&nbsp;&nbsp;" + (input.files[i].size / (1024 * 1024)).toFixed(2) + "MB";
        fileList.appendChild(li);
    }
}

function requestNextMusic(playlist) {
    console.log(Math.floor(Math.random() * playlist.length));
}

window.addEventListener("message",(event)=>{
    if(event.data == "Close")
        alert("Hello world");
});