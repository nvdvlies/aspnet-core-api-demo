import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { Permission } from '@shared/enums/permission.enum';
import { UserPermissionService } from '@shared/services/user-permission.service';

@Directive({
  selector: '[appIfPermissionDenied]'
})
export class IfPermissionDeniedDirective {
  constructor(
    private readonly templateRef: TemplateRef<any>,
    private readonly viewContainerRef: ViewContainerRef,
    private readonly userPermissionService: UserPermissionService
  ) {}

  @Input() set appIfPermissionDenied(permission: keyof typeof Permission) {
    const hasPermission = this.userPermissionService.hasPermission(permission);
    if (hasPermission) {
      this.viewContainerRef.clear();
    } else {
      this.viewContainerRef.createEmbeddedView(this.templateRef);
    }
  }
}
