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
import { ApiUsersClient, UserLookupOrderByEnum } from '@api/api.generated.clients';
import {
  AutocompleteMatFormFieldControlBase,
  AutocompleteOption,
  IAutocompleteOption
} from '@shared/base/autocomplete-mat-form-field-control-base';
import { UserLookupService } from '@shared/services/user-lookup.service';
import { map } from 'rxjs';

@Component({
  selector: 'app-user-autocomplete-form-field-control',
  templateUrl: '../../base/autocomplete-mat-form-field-control-base.html',
  styleUrls: [
    '../../base/autocomplete-mat-form-field-control-base.scss',
    './user-autocomplete-form-field-control.component.scss'
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: UserAutocompleteFormFieldControlComponent
    }
  ]
})
export class UserAutocompleteFormFieldControlComponent extends AutocompleteMatFormFieldControlBase<IAutocompleteOption> {
  constructor(
    @Optional() elementRef: ElementRef<HTMLElement>,
    @Optional() focusMonitor: FocusMonitor,
    @Host() @SkipSelf() controlContainer: ControlContainer,
    @Optional() @Self() ngControl: NgControl,
    private readonly apiUsersClient: ApiUsersClient,
    private readonly userLookupService: UserLookupService
  ) {
    super(elementRef, focusMonitor, controlContainer, ngControl);
  }

  protected searchFunction = (searchTerm: string | undefined) => {
    return this.apiUsersClient
      .lookup(UserLookupOrderByEnum.Fullname, false, 0, 25, searchTerm, undefined)
      .pipe(
        map(
          (response) =>
            response.users?.map((user) => new AutocompleteOption(user.id!, user.fullname!)) ?? []
        )
      );
  };

  protected lookupFunction = (id: string) => {
    return this.userLookupService
      .getById(id)
      .pipe(map((user) => (user ? new AutocompleteOption(user.id!, user.fullname!) : undefined)));
  };
}
