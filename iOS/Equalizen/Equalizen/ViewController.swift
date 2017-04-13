//
//  ViewController.swift
//  Equalizen
//
//  Created by MorisHenry on 4/6/17.
//  Copyright © 2017 altair. All rights reserved.
//

import UIKit
import AVFoundation

class ViewController: UIViewController {
    
    @IBOutlet weak var progressView: UIProgressView!
    
    var audioPlayer = AVAudioPlayer()

    override func viewDidLoad() {
        super.viewDidLoad()
        
        do {
            audioPlayer = try AVAudioPlayer(contentsOf: URL.init(fileURLWithPath: Bundle.main.path(forResource: "여대앞에사는남자", ofType: "mp3")!))
            audioPlayer.prepareToPlay()
            
            let audioSession = AVAudioSession.sharedInstance()
            
            do {
                try audioSession.setCategory(AVAudioSessionCategoryPlayback)
                
                Timer.scheduledTimer(timeInterval: 0.1, target: self, selector: #selector(ViewController.UpdateProgressView), userInfo: nil, repeats: true)
                progressView.setProgress(Float(audioPlayer.currentTime / audioPlayer.duration), animated: false)
            }
            
        } catch {
            NSLog("Error")
        }
    }
    
    func UpdateProgressView(){
        
        if audioPlayer.isPlaying {
            progressView.setProgress(Float(audioPlayer.currentTime/audioPlayer.duration), animated: true)
        }
        
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }
    
    @IBAction func Play(_ sender: Any) {
        audioPlayer.play()
    }
    
    @IBAction func Replay(_ sender: Any) {
        if audioPlayer.isPlaying {
            audioPlayer.currentTime = 0.0
            audioPlayer.play()
        } else {
            
        }
    }

    @IBAction func Pause(_ sender: Any) {
        if audioPlayer.isPlaying {
            audioPlayer.pause()
        }
    }
}

