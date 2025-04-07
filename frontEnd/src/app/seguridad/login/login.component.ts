import { Component, OnInit } from '@angular/core';
import { SeguridadService } from '../seguridad.service';
import { Route, Router } from '@angular/router';
import { credencialesUsuario } from '../seguridad';
import { parsearErroresAPI } from 'src/app/utilidades/utilidades';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private seguridadService: SeguridadService, private router: Router) { }

  errores: string[] = []
  ngOnInit(): void {
  }

  login(credenciales: credencialesUsuario){
    this.seguridadService.login(credenciales)
      .subscribe(respuesta => {
        this.seguridadService.guardarToken(respuesta);
        this.router.navigate(['/'])
      }, errores => this.errores = parsearErroresAPI(errores))
  }

}
