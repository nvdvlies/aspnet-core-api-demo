import { Directive, HostListener, Input } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';

@Directive({
  selector: '[appBackButton]'
})
export class BackButtonDirective {
  @Input()
  fallbackRouterLink: string | undefined;

  constructor(private router: Router, private location: Location) {}

  @HostListener('click', ['$event.target'])
  onClick(): void {
    if (this.router.navigated) {
      this.location.back();
    } else if (this.fallbackRouterLink) {
      this.router.navigateByUrl(this.fallbackRouterLink);
    } else {
      this.router.navigateByUrl('/');
    }
  }
}
