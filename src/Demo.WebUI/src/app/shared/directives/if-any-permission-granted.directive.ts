import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

@Directive({
  selector: '[appIfAnyPermissionGranted]'
})
export class IfAnyPermissionGrantedDirective {
  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly userPermissionService: UserPermissionService
  ) {}

  @Input() set appIfAnyPermissionGranted(permissions: Array<keyof typeof Permission>) {
    const hasPermission = this.userPermissionService.hasAnyPermission(permissions);
    if (hasPermission) {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    } else {
      this.viewContainerRef.clear();
    }
  }
}
