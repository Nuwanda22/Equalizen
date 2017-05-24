//buil an equalizer with multiple biquad filters
var isFocus = true;
var isPlayed = true;

var ctx = window.AudioContext || window.webkitAudioContext;
var context = new ctx();
var mediaElement = document.getElementById('player');

var sourceNode = context.createMediaElementSource(mediaElement);

// create the equalizer. It's a set of biquad Filters

var filters = [];

// Set filters
[60, 170, 350, 1000, 3500, 10000].forEach(function (freq, i) {
  var eq = context.createBiquadFilter();
  eq.frequency.value = freq;
  eq.type = "peaking";
  eq.gain.value = 0;
  filters.push(eq);
});

// Connect filters in serie
sourceNode.connect(filters[0]);
for (var i = 0; i < filters.length - 1; i++) {
  filters[i].connect(filters[i + 1]);
}

// connect the last filter to the speakers
filters[filters.length - 1].connect(context.destination);

function changeGain(sliderVal, nbFilter) {
  var value = parseFloat(sliderVal);
  filters[nbFilter].gain.value = value;

  // update output labels
  var output = document.querySelector("#gain" + nbFilter);
  output.value = value + " dB";
}

window.onfocus = () => {
  isFocus = true;
}

window.onblur = () => {
  isFocus = false;
}

// change play state with space bar
window.onkeypress = (event) => {

  // exception can't change status when focus on play state button
  if (document.activeElement.type === "button") {
    document.activeElement.blur();
  }

  // check key press space bar and focus on this page
  if (event.code === "Space" && isFocus) {
    // check song played and change play state
    if (isPlayed) {
      mediaElement.play();
      document.getElementById('play-state').innerHTML = "⏸";
    } else if (!isPlayed) {
      mediaElement.pause();
      document.getElementById('play-state').innerHTML = "▶";
    }

    isPlayed = !isPlayed
  }
}

// ended song event listener, I should implement get next song function
mediaElement.addEventListener('ended', () => {
  mediaElement.currentTime = 0;
  mediaElement.play()
  console.log('ended');
});

// play state button on click function
document.getElementById('play-state').onclick = (event) => {
  console.log(isPlayed);
  if (isPlayed) {
    mediaElement.play();
    event.target.innerHTML = "⏸";
  } else if (!isPlayed) {
    mediaElement.pause();
    event.target.innerHTML = "▶";
  }
  isPlayed = !isPlayed
}

// code reference from codepen 'http://codepen.io/isokol/pen/PZmxGO'