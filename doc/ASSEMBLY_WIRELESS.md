# How to make a Hotdog (wireless version)

## Parts list

### Custom parts
3D print the following modules:
- 1x [hotdog-case-top](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-case-top.stl)
- 1x [hotdog-case-bottom](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-case-bottom.stl)

Order the following items from a PCB fabrication service (estimated cost: 10-20 USD each):
- 1x [hotdog-pcb-front](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-pcb-front-fab.zip),
size: 100x50mm, layers: 2, thickness: 1.6mm, solder mask: yes, HASL: yes
- 1x [hotdog-pcb-back](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-pcb-back-fab.zip),
size: 100x50mm, layers: 2, thickness: 1.6mm, solder mask: yes, HASL: yes


### Stock parts
⚠ For the USB connected version, only parts A1, D1-D3, R1-R6 and J1-J4 are required (highlighted below).

- **A1: Arduino Nano (a Nano Every also works) + two 1x15 pin female headers**
- BT1: an AAA battery holder
- C1: a 10-100uF capacitor (optional)
- C2: a ~1000uF capacitor (optional)
- D1-D3: three white LEDs (I used 334-15/T1C1-4WYA)
- **a 12cm long hook-up wire connecting J1 to J4**
- **a 12cm long hook-up wire connecting J2 to J3**
- **R1-R5: five GL5528 photoresistors**
- **R6: 1kΩ resistor (or lower if you're using less bright LEDs)**
- SW1: SPDT switch
- U1: nRF24L01 + a 2x4 pin female header
- U2: 0.8-5V to 5V boost converter + a 1x3 pin female header
- Mini zip ties
- Rubber bands


## Assembly

### Programming
Arduino Nano:

Extract [this zip](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-fw-nano-bin.zip) and upload the file `hotdog-fw.ino.hex` to the Nano using [XLoader](https://www.hobbytronics.co.uk/arduino-xloader).

Other Nano-compatible microcontrollers (Nano Every etc.):

Compile and upload [this sketch](https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-fw.ino) using the [Arduino IDE](https://www.arduino.cc/en/software). The firmware depends on the following libraries:
- BTLE
- Protothreads
- RF24


### Soldering
Solder every part into its designated place on the motherboards. More detailed instructions coming soon.


### Final assembly
1. Slide **hotdog-pcb-front** into **hotdog-case-top** and **hotdog-pcb-back** into **hotdog-case-bottom**. Make sure the LEDs and the photoresistors all face inwards (where the sleeve will be).
2. Secure each motherboard with a zip tie and the cut-off end of another zip tie.
3. Add the sleeve.
4. Bind the two parts of the chassis together at their open ends with zip ties. Fasten the zip ties just tight enough that the chassis won't separate from the sleeve.
5. Secure the other end of the chassis to the sleeve with rubber bands.
6. Insert battery, power on and serve.


## Operation
In wireless mode, the Hotdog is powered by a single AAA battery. I recommend using a rechargeable one. It should last you about 2-3 hours on a single charge. There's no battery level indicator, so recharge frequently.

I'm still working on the client app for wireless, but it's already looking great. Stay tuned!