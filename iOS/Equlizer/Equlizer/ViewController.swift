//
//  ViewController.swift
//  Equalize
//
//  Created by MorisHenry on 4/20/17.
//  Copyright © 2017 altair. All rights reserved.
//

import UIKit
import MediaPlayer

class ViewController: UIViewController {
    
    @IBOutlet weak var albumCover: UIImageView!
    @IBOutlet weak var titleLabel: UILabel!
    @IBOutlet weak var nameLabel: UILabel!
    @IBOutlet weak var actionTarget: UIButton!
    
    let musicPlayer = MPMusicPlayerController()
    let mediaQuery = MPMediaQuery.songs()

    override func viewDidLoad() {
        super.viewDidLoad()
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
    }
    
    func SetupLabels() {
        
        titleLabel.text = musicPlayer.nowPlayingItem?.title
        
        if musicPlayer.nowPlayingItem?.artwork?.image == nil {
            albumCover.image = nil
            NSLog("Error : No Album Cover On This Song!")
        } else {
            albumCover.image = musicPlayer.nowPlayingItem?.artwork?.image(at: albumCover.bounds.size)
        }
        
        if musicPlayer.nowPlayingItem?.artist == nil {
            nameLabel.text = "Unkown Artist"
        } else {
            nameLabel.text = musicPlayer.nowPlayingItem?.artist
        }
        
    }

    @IBAction func Action(_ sender: UIButton) {
        let senderType = sender.currentTitle
        
        switch senderType! {
            
        case "NEXT":
            musicPlayer.setQueue(with: mediaQuery)
            musicPlayer.play()
            
            SetupLabels()
            
            break
            
        case "PLAY":
            musicPlayer.play()
            actionTarget.setTitle("PAUSE", for: UIControlState.normal)
            
            SetupLabels()
            
            break
            
        case "PAUSE":
            musicPlayer.pause()
            actionTarget.setTitle("PLAY", for: UIControlState.normal)
            break
            
        default: break
        }
    }

}

