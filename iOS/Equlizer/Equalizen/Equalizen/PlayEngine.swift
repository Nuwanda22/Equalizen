//
//  PlayEngine.swift
//  Equalizen
//
//  Created by MorisHenry on 6/1/17.
//  Copyright © 2017 com.altair. All rights reserved.
//

import AVFoundation

public class PlayEngine: NSObject {
    
    private var componentType: UInt32 {
        
        willSet {
            stopPlaying()
            
            if testUnitNode != null {
            
                if isEffect() { engine.disconnectNodeInput(testUnitNode!) }
                
                engine.disconnectNodeInput(engine.mainMixerNode)
                engine.detach(testUnitNode!)
                
                instrumentPlayer = nil
                
                testUnitNode = nil
                testAudioUnit = nil
                presetList = nil
            }
        } didSet {
            if componentType != kAudioUnitType_Effect && componentType != kAudioUnitType_MusicDevice {
                componentType = kAudioUnitType_Effect
            }
            
            if isEffect() {
                engine.connect(player, to: engine.mainMixerNode, format: file!.processingFormat)
            }
            
            if componentsFoundCallback != nil {
                updateAudioUnitList()
            }
        }
    }
    
    public var testAudioUnit: AUAudioUnit?
    var presetList = [AUAudioUnitPreset]()
    
    private let stateChangeQueue = DispatchQueue(label: "SimplePlayEngine.stateChangeQueue")
    private let engine = AVAudioEngine()
    private let player = AVAudioPlayerNode()
    private var instrumentPlayer: InstrumentPlayer? = nil;
    private var testUnitNode: AVAudioUnit?
    private var file: AVAudioFile?
    private var isPlaying = false
    private let componentsFoundCallback: ((Void)->Void)?
    private let availableAudioUnitsAccessQueue = DispatchQueue(label: "PlayEngine.availableAudioUnitsAccessQueue")
    
    private var _availableAudioUnits = [AVAudioUnitComponent]()
    
    func isEffect() -> Bool { return componentType == kAudioUnitType_Effect }
    func isInstrument() -> Bool { return componentType == kAudioUnitType_MusicDevice }
    
    private func updateAudioUnitList() {
        DispatchQueue.global(qos: .default).async {
            var componentDescription = AudioComponentDescription()
            
            componentDescription.componentType = self.componentType
            componentDescription.componentSubType = 0
            componentDescription.componentManufacturer = 0
            componentDescription.componentFlags = 0
            componentDescription.componentFlagsMask = 0
            
            self._availableAudioUnits = AVAudioUnitComponentManager.shared().components(matching: componentDescription)
            
            DispatchQueue.main.async {
                self.componentsFoundCallback!()
            }
        }
    }
}
