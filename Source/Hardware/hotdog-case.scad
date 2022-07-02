$fn = 50;

t = 0.7; // tolerance
pcb = [50 + t, 1.5 + t, 100 + t];

big = 420; // arbitrary big value

module pcb_clearance(h_top) {
    h_bottom = 5;
    h_slit = 30;
    w_slit = 10;
    margin = 4;
    cube(pcb, center = true);
    translate([0, -h_top / 2, 0])
        cube([pcb[0] - margin, h_top, pcb[2] - margin], center = true);
    translate([0, h_bottom / 2, 0])
        cube([pcb[0] - margin, h_bottom, pcb[2] - margin], center = true);
    translate([0, h_slit / 2, 0])
        cube([w_slit, h_slit, pcb[2]], center = true);
    translate([0, -h_top / 2, pcb[2] / 2])
        cube([pcb[0], h_top, 10], center = true);
    translate([0, big / 2, pcb[2] / 2])
        cube([pcb[0], big, 10], center = true);
    translate([0, 0, pcb[2] / 2 + 1])
        cube([big, 3, 2], center = true);
}

module sleeve_clearance() {
    sleeve_d = 50;
    translate([0, 32, 0])
    cylinder(d = sleeve_d, h = pcb[2] + 20, center = true);
}

module ear() {
    ear_h = 15;
    intersection() {
        difference() {
            cube([8, 8, ear_h], center = true);
            cube([3, 2, ear_h], center = true);
        }
        translate([4, 0, 0])
            rotate([90, 0, 0])
                cylinder(d = ear_h, h = 100, center = true);
    }
}

module hook() {
    ear_h = 16;
        difference() {
            cube([8, 12, ear_h], center = true);
            translate([2, -2, 0])
                cube([4, 8, big], center = true);
        }
}

module case(h_top, battery_hole) {
    difference() {
        translate([0, (25 - h_top) / 2, 0])
            cube([pcb[0] + 5, 30 + h_top, pcb[2] + 10], center = true);
        pcb_clearance(h_top);
        sleeve_clearance();
        if (battery_hole) {
            hull() {
                rotate([90, 0, 0]) {
                    translate([-10, -pcb[2] / 2 + 20, 0])
                        cylinder(d = 20, h = big);
                    translate([-10, 10, 0])
                        cylinder(d = 20, h = big);
                }
            }
        }
    }
    corner = [- pcb[0] / 2 - 6, 20, pcb[2] / 2 - 3];
    module ear_corner() {
        translate(corner)
            ear();
    }
    module hook_corner() {
        translate(corner)
            hook();
    }
    ear_corner();
    mirror([1, 0, 0])
        ear_corner();
    mirror([0, 0, 1])
        hook_corner();
    mirror([1, 0, 0])
        mirror([0, 0, 1])
            hook_corner();
}

module hotdog_bottom() {
    case(10, false);
}

module hotdog_top() {
    case(20, true);
}
