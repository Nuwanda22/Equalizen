//
//  ViewController.swift
//  Equalizen
//
//  Created by MorisHenry on 4/6/17.
//  Copyright © 2017 altair. All rights reserved.
//

import UIKit
import MediaPlayer

class ViewController: UIViewController {
    
    @IBOutlet weak var statusText: UILabel!
    @IBOutlet weak var actionButton: UIButton!
    @IBOutlet weak var nextButton: UIButton!
    @IBOutlet weak var prevButton: UIButton!
    
    let musicPlayer = MPMusicPlayerController()
    let mediaQuery = MPMediaQuery.songs()
    
    override func viewDidLoad() {
        super.viewDidLoad()
        
        statusText.text = musicPlayer.nowPlayingItem?.title
        
        NSLog("View Loaded")
    }
    
    @IBAction func Action(_ sender: UIButton) {
        
        NSLog("Button Tapped")
        
        let currentTitle = sender.currentTitle
        
        switch currentTitle! {
        case "NEXT":
            musicPlayer.setQueue(with: mediaQuery)
            musicPlayer.play()
            statusText.text = musicPlayer.nowPlayingItem?.title
            actionButton.setTitle("PAUSE", for: UIControlState.normal)
            break
            
        case "PLAY":
            musicPlayer.play()
            statusText.text = musicPlayer.nowPlayingItem?.title
            actionButton.setTitle("PAUSE", for: UIControlState.normal)
            break
            
        case "PAUSE":
            musicPlayer.pause()
            actionButton.setTitle("PLAY", for: UIControlState.normal)
            break
            
        default:
            break
        }
    }
    
    
    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
}

