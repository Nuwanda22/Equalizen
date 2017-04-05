//
//  ViewController.swift
//  Equalier
//
//  Created by MorisHenry on 4/5/17.
//  Copyright © 2017 altair. All rights reserved.
//

import UIKit
import AVFoundation

class ViewController: UIViewController {
    
    @IBOutlet weak var progressView: UIProgressView!
    
    var audioPlayer: AVAudioPlayer!

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        
        do {
            audioPlayer = try AVAudioPlayer(contentsOf: URL.init(fileURLWithPath: Bundle.main.path(forResource: "sample", ofType: "mp3")!))
            audioPlayer.prepareToPlay()
            
            Timer.scheduledTimer(timeInterval: 1.0, target: self, selector: #selector(ViewController.updateProgressView), userInfo: nil, repeats: true)
            progressView.setProgress(Float(audioPlayer.currentTime / audioPlayer.duration), animated: false)
        }
        catch {
            print(error)
        }
    }
    
    func updateProgressView() {
        if audioPlayer.isPlaying {
            progressView.setProgress(Float(audioPlayer.currentTime / audioPlayer.duration), animated: true)
        }
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }

    @IBAction func Pause(_ sender: AnyObject) {
        
        if audioPlayer.isPlaying
        {
            audioPlayer.pause()
            print("Current Time : ", audioPlayer.currentTime)
        }
        
    }

    @IBAction func Play(_ sender: AnyObject) {
        audioPlayer.play()
    }
    
    @IBAction func Replay(_ sender: AnyObject) {
        
        if audioPlayer.isPlaying
        {
            audioPlayer.currentTime = 0.0
            progressView.progress = 0.0
            audioPlayer.play()
            print("Current Time : %f", audioPlayer.currentTime)
        }
            
        else if !audioPlayer.isPlaying
        {
            audioPlayer.currentTime = 0.0
            progressView.progress = 0.0
            audioPlayer.play()
            print("Current Time : %f", audioPlayer.currentTime)
        }
        
    }
}

