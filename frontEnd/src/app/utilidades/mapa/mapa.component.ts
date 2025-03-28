import { Component, EventEmitter, OnDestroy, OnInit} from '@angular/core';
import * as L from 'leaflet';

@Component({
  selector: 'app-mapa',
  templateUrl: './mapa.component.html',
  styleUrls: ['./mapa.component.css']
})
export class MapaComponent implements OnInit, OnDestroy {
  map: L.Map | undefined;
  markers: L.Marker[] = [];
  constructor() { }

  ngOnInit(): void {
    this.initializeMap();
  }

  ngOnDestroy(): void {
    if(this.map){
      this.map.remove();
    }
  }

  private initializeMap(): void{
    this.map = L.map('map').setView([40.4168, -3.7038], 13);

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution:'&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    }).addTo(this.map);

    this.map.on('click', (e: L.LeafletMouseEvent) => this.onMapClick(e));
  }

  private onMapClick(event: L.LeafletMouseEvent): void{
    const latLng = event.latlng;
    const marker = L.marker([latLng.lat, latLng.lng]);
    
    marker.addTo(this.map).bindPopup(`${latLng.lat.toFixed(4)}, ${latLng.lng.toFixed(4)}`).openPopup();
    
    

    this.markers.push(marker);
    
  }
}
