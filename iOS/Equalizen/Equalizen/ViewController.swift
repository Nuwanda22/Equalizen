//
//  ViewController.swift
//  Equalizen
//
//  Created by MorisHenry on 3/30/17.
//  Copyright © 2017 altair. All rights reserved.
//

import UIKit
import AVFoundation

class ViewController: UIViewController {
    
    var audioPlayer = AVAudioPlayer

    override func viewDidLoad() {
        super.viewDidLoad()
        // Do any additional setup after loading the view, typically from a nib.
        
        do {
            audioPlayer = try AVAudioPlayer(contentsOf: Bundle.main)
        }
        catch {
            print(error)
        }
    }

    override func didReceiveMemoryWarning() {
        super.didReceiveMemoryWarning()
        // Dispose of any resources that can be recreated.
    }


}

