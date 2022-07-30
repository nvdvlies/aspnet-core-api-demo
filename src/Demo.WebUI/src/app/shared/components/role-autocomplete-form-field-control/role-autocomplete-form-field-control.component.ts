import { FocusMonitor } from '@angular/cdk/a11y';
import {
  Component,
  ChangeDetectionStrategy,
  Optional,
  ElementRef,
  Host,
  SkipSelf,
  Self
} from '@angular/core';
import { ControlContainer, NgControl } from '@angular/forms';
import { MatFormFieldControl } from '@angular/material/form-field';
import { ApiRolesClient, RoleLookupOrderByEnum } from '@api/api.generated.clients';
import {
  AutocompleteMatFormFieldControlBase,
  AutocompleteOption,
  IAutocompleteOption
} from '@shared/base/autocomplete-mat-form-field-control-base';
import { RoleLookupService } from '@shared/services/role-lookup.service';
import { map } from 'rxjs';

@Component({
  selector: 'app-role-autocomplete-form-field-control',
  templateUrl: '../../base/autocomplete-mat-form-field-control-base.html',
  styleUrls: [
    '../../base/autocomplete-mat-form-field-control-base.scss',
    './role-autocomplete-form-field-control.component.scss'
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: RoleAutocompleteFormFieldControlComponent
    }
  ]
})
export class RoleAutocompleteFormFieldControlComponent extends AutocompleteMatFormFieldControlBase<IAutocompleteOption> {
  constructor(
    @Optional() elementRef: ElementRef<HTMLElement>,
    @Optional() focusMonitor: FocusMonitor,
    @Host() @SkipSelf() controlContainer: ControlContainer,
    @Optional() @Self() ngControl: NgControl,
    private readonly apiRolesClient: ApiRolesClient,
    private readonly roleLookupService: RoleLookupService
  ) {
    super(elementRef, focusMonitor, controlContainer, ngControl);
  }

  protected searchFunction = (searchTerm: string | undefined) => {
    return this.apiRolesClient
      .lookup(RoleLookupOrderByEnum.Name, false, 0, 25, searchTerm, undefined)
      .pipe(
        map(
          (response) =>
            response.roles?.map((role) => new AutocompleteOption(role.id!, role.name!)) ?? []
        )
      );
  };

  protected lookupFunction = (id: string) => {
    return this.roleLookupService
      .getById(id)
      .pipe(map((role) => (role ? new AutocompleteOption(role.id!, role.name!) : undefined)));
  };
}
