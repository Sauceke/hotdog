

# Wireless Assembly

*How to build an **IOT Hotdog**.*

<br>

## Parts list

<img
  src = '../Resources/Wireless/parts.jpeg'
  width = 420
/>

<br>

### Custom Parts

#### 3D Print

- **[Top Casing]**

- **[Bottom Casing]**

<br>

#### Order PCBs

Estimated Cost : `10 - 20 US$` per PCB

- **[Front PCB]**

- **[Back PCB]**

###### Specifications:

- Solder Mask : `Yes`
- Thickness : `1.6mm`
- Layers: `2`
- Size : `100 x 50 mm`
- HASL : `Yes`

<br>

### Stock Parts

- **`A1` : Arduino Nano or compatible + two `1x15` pin female headers**

- `BT1` : AAA battery holder (thru-hole mount recommended)

- `C1` : `10 - 100uF` capacitor (optional)

- `C2` : `~1000uF` capacitor (optional)

- **`D1-D3` : three white LEDs, I used `334-15` / `T1C1-4WYA`**

- **`12cm` long hook-up wire connecting `J1` to `J4`**

- **`12cm` long hook-up wire connecting `J2` to `J3`**

- **`R1-R5` : five GL5528 photoresistors**

- **`R6` : 1kΩ resistor (or lower if you're using less bright LEDs)**

- `SW1` : **SPDT** switch

- `U1` : `nRF24L01` + a `2x4` pin female header

- `U2` : `0.8 - 5V` to `5V` boost converter + a `1x3` pin female header

- A transparent sleeve, I used **[This][Sleeve]** one.

- Mini zip ties

- Rubber bands

<br>
<br>

## Programming

### Arduino Nano

Extract this **[Zip]** file and upload  
the file `hotdog-fw.ino.hex` to  
the Nano using **[XLoader]**.

<img
  src = '../Resources/Wireless/xloader.png'
  width = 210
/>

##### Windows

By default you should be able to  
use `COM0`, if not try other ports.

<br>

### Other 

*Nano-compatible microcontrollers*

Compile & upload the **[Firmware]** using the **[Arduino IDE]**.

##### Dependencies

- **Protothreads**
- **BTLE**
- **RF24**

<br>
<br>

## Assembly

### Soldering Font PCB

1.  Place the `1x15` headers on the Nano.

    *I didn't have 1x15, so I cheated and yanked*  
    *out one of the terminals from a `1x16`.*
    
    *Don't be like me.*

    <img
      src = '../Resources/Wireless/A1_prep.jpeg'
      height = 200
    />
    
    <br>

2.  Solder the micro-controller to the PCB with  
    its USB connector facing towards the edge.

    <img
      src = '../Resources/Wireless/A1_mount.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/A1_solder.jpeg'
      height = 200
    />
    
    <br>

3.  Solder the photo-resistors to `R1-R5`  
    on the opposite side of the PCB.

    <img
      src = '../Resources/Wireless/R1-R5_mount.jpeg'
      height = 200
    />

    <br>

    *It is recommended to clip the leads off*  
    *before soldering to minimize heat loss.*
    
    I prefer to also bend the wires in before clipping <br>
    them short, so the parts won't come loose.

    <img
      src = '../Resources/Wireless/R1-R5_mount2.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/R1-R5_solder.jpeg'
      height = 200
    />
    
    <br>

4.  Solder the `2x4` header to `U1`.

    <img
      src = '../Resources/Wireless/U1_mount.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/U1_solder.jpeg'
      height = 200
    />
    
    <br>

5.  Put the `1x3` female header on the  
    boost converter and solder it to `U2`.

    Make sure `V1` and `V0` are aligned  
    with the markings on the PCB.

    <img
      src = '../Resources/Wireless/U2_prep.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/U2_mount.jpeg'
      height = 200
    />
    
    <br>

6.  Solder a capacitor of about `1000uF` to `C2`.

    ***Mind the polarity.***

    <img
      src = '../Resources/Wireless/C2_mount.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/C2_solder.jpeg'
      height = 200
    />
    
    <br>

7.  Solder the **SPDT** switch to `SW1`.

    The terminals should go into the three holes in the middle.
    
    Mine was a bit over-sized, but as they  
    say, if there's a hole, there's a way.

    <img
      src = '../Resources/Wireless/SW1_mount.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/SW1_solder.jpeg'
      height = 200
    />

    Words to live by indeed.
    
    <br>

8.  If you have a thru-hole mount `1xAAA` battery  
    holder, you can go ahead and solder it to `BT1`  
    and skip to step 13.

    ***Mind the polarity***

    If you're using a cheaper battery holder like  
    I did, we need to take a few extra steps.
    
    This can get quite messy though, so in  
    retrospect I absolutely recommend  
    getting a proper thru-hole mount.
    
    <br>

9.  First, trim both wires to about `2cm`, and put some  
    deep scratches into the bottom with a sharp object.

    This is crucial to ensure proper adhesion  
    to the board, so carve it up like a psycho.

    <img
      src = '../Resources/Wireless/BT1_prep.jpeg'
      height = 200
    />
    
    <br>

10. Then solder it to `BT1` in the correct polarity.

    <img
      src = '../Resources/Wireless/BT1_solder.jpeg'
      height = 200
    />
    
    <br>

11. Clean both the `BT1` footprint and the  
    battery holder with isopropyl alcohol.

12. Glue the battery holder to `BT1` with epoxy.

    ***⚠ Make sure there is at least `5mm` clearance***  
    ***between the battery holder and the edge of***  
    ***the motherboard.***
    
    *It doesn't have to be precisely on the footprint,*  
    *but do not place it any further below that.*

    <img
      src = '../Resources/Wireless/BT1_mount.jpeg'
      height = 200
    />
    <img
      src = '../Resources/Wireless/BT1_mount2.jpeg'
      height = 200
    />

    Clamp it down and wait for the glue to dry.
    
    Meanwhile, you can start assembling the [Back PCB](#soldering-back-pcb).
    
    <br>

13. Solder a 100uF capacitor to `C1`.

    ***Mind the polarity.***

    <img
      src = '../Resources/Wireless/C1_mount.jpeg'
      height = 200
    />
    
    <br>

14. Insert the `nRF24` into the `U1` socket,  
    and that's the front motherboard done.

    <img
      src = '../Resources/Wireless/FRONT_done.jpeg'
      width = 300
    />
    <img
      src = '../Resources/Wireless/FRONT_done2.jpeg'
      width = 300
    />

<br>
<br>

### Soldering Back PCB

1.  Solder the LEDs to `D1-D3`.

    Pay attention to polarity; one of the sides  
    on each LED is flattened, line that up with  
    the drawing on the PCB.
      
    <img
      src = '../Resources/Wireless/D1-D3_mount.jpeg'
      height = 160
    />
    <img
      src = '../Resources/Wireless/D1-D3_solder.jpeg'
      height = 160
    />
    
    <br>
  
2.  Solder a `1K` resistor to `R6`.

    <img
      src = '../Resources/Wireless/R6_mount.jpeg'
      height = 200
    />
    
    <br>

3.  Connect `J1` to `J4` and `J2` to `J3`  
    with `12cm` long hook-up wires.

    Solder those in as well.

    <img
      src = '../Resources/Wireless/J1-J4_mount.jpeg'
      height = 200
    />

    The motherboards are now complete.

    <img
      src = '../Resources/Wireless/BACK_done.jpeg'
      height = 200
    />

<br>
<br>

### Final Assembly

1.  Slide the PCB into the casing:

    **Front PCB**  ➞  **Top Casing**

    **Bottom PCB**  ➞  **Bottom Casing**

    *Make sure the LEDs and the photo-resistors*  
    *all face inwards, where the sleeve will be.*

2.  Secure each PCB with a zip tie and  
    the cut-off end of another zip tie.

3.  Add the sleeve.

4.  Use zip ties to bind the two casing  
    parts at their open ends together.

    Fasten the zip ties just tight enough that  
    the casing won't separate from the sleeve.

5.  Secure the other end of the casing  
    to the sleeve with rubber bands.

6.  Insert the battery, power it on and serve.

<br>
<br>

## Operation

In wireless mode, the **Hotdog** is powered by a single  
AAA battery that should last you about `2 - 3` hours  
on a single charge.

*I recommend using a rechargeable one.*

*There's no battery level indicator, so recharge frequently.*

I'm still working on the client app for  
wireless, but it's already looking great.

Stay tuned!

<br>


<!----------------------------------------------------------------------------->

[Firmware]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-fw.ino
[Zip]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-fw-nano-bin.zip

[Arduino IDE]: https://www.arduino.cc/en/software
[XLoader]: https://www.hobbytronics.co.uk/arduino-xloader


<!---------------------------------[ Parts ]----------------------------------->

[Bottom Casing]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-case-bottom.stl
[Top Casing]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-case-top.stl

[Front PCB]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-pcb-front-fab.zip
[Back PCB]: https://github.com/Sauceke/hotdog/releases/latest/download/hotdog-pcb-back-fab.zip

[Sleeve]: https://www.thehandy.com/product/the-handy-open-ended-sleeve-collection-hard-eu-uk-2/?ref=saucekebenfield&utm_source=saucekebenfield&utm_medium=affiliate&utm_campaign=The%20Handy%20Affiliate%20program