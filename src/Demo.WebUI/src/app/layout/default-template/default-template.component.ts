import { Component, OnInit } from '@angular/core';
import { AuthService } from '@auth0/auth0-angular';

@Component({
  templateUrl: './default-template.component.html',
  styleUrls: ['./default-template.component.scss']
})
export class DefaultTemplateComponent implements OnInit {

  constructor(
    public authService: AuthService
  ) {
  }

  ngOnInit(): void {
  }

  public logout(): void {
    this.authService.logout();
  }
}
