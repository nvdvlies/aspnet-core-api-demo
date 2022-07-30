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
import { ILocationDto } from '@api/api.generated.clients';
import {
  AutocompleteMatFormFieldControlBase,
  IAutocompleteOption
} from '@shared/base/autocomplete-mat-form-field-control-base';
import { LocationLookupService } from '@shared/services/location-lookup.service';
import {
  LocationSearchRequest,
  LocationSearchService
} from '@shared/services/location-search.service';
import { map } from 'rxjs';
import { v4 as uuidv4 } from 'uuid';

export interface ILocationAutocompleteOption extends ILocationDto, IAutocompleteOption {}

@Component({
  selector: 'app-location-autocomplete-form-field-control',
  templateUrl: '../../base/autocomplete-mat-form-field-control-base.html',
  styleUrls: [
    '../../base/autocomplete-mat-form-field-control-base.scss',
    './location-autocomplete-form-field-control.component.scss'
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: MatFormFieldControl,
      useExisting: LocationAutocompleteFormFieldControlComponent
    }
  ]
})
export class LocationAutocompleteFormFieldControlComponent extends AutocompleteMatFormFieldControlBase<ILocationAutocompleteOption> {
  constructor(
    @Optional() elementRef: ElementRef<HTMLElement>,
    @Optional() focusMonitor: FocusMonitor,
    @Host() @SkipSelf() controlContainer: ControlContainer,
    @Optional() @Self() ngControl: NgControl,
    private readonly locationSearchService: LocationSearchService,
    private readonly locationLookupService: LocationLookupService
  ) {
    super(elementRef, focusMonitor, controlContainer, ngControl);
  }

  protected searchFunction = (searchTerm: string | undefined) => {
    const request: LocationSearchRequest = {
      searchTerm
    };
    return this.locationSearchService.search(request).pipe(
      map(
        (response) =>
          response.locations?.map((location) => {
            const autocompleteOption: ILocationAutocompleteOption = {
              id: uuidv4(),
              label: location.displayName!,
              ...location
            };
            return autocompleteOption;
          }) ?? []
      )
    );
  };

  protected lookupFunction = (id: string) => {
    return this.locationLookupService.getById(id).pipe(
      map((location) => {
        if (!location) {
          return undefined;
        }
        const autocompleteOption: ILocationAutocompleteOption = {
          label: location.displayName!,
          ...location
        };
        return autocompleteOption;
      })
    );
  };
}
