import { Component, OnInit } from '@angular/core';
import { DataFormatService } from './services/data-format.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  constructor(
  ){}
  
  ngOnInit(): void {
    
  }
  title = 'front-end';
  
}
