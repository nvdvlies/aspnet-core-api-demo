import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ILocationDto } from '@api/api.generated.clients';
import { map, Observable, of } from 'rxjs';

export interface LocationSearchRequest {
  searchTerm: string | undefined;
}

export interface LocationSearchResponse {
  locations: ILocationDto[];
}

interface ExternalSourceReponse {
  response: {
    numFound: number;
    start: number;
    maxScore: number;
    docs: [
      {
        bron: string;
        woonplaatscode: string;
        type: string;
        woonplaatsnaam: string;
        huis_nlt: string;
        openbareruimtetype: string;
        gemeentecode: string;
        weergavenaam: string;
        straatnaam_verkort: string;
        id: string;
        gemeentenaam: string;
        identificatie: string;
        openbareruimte_id: string;
        provinciecode: string;
        postcode: string;
        provincienaam: string;
        centroide_ll: string;
        nummeraanduiding_id: string;
        adresseerbaarobject_id: string;
        huisnummer: string;
        provincieafkorting: string;
        centroide_rd: string;
        straatnaam: string;
        gekoppeld_perceel: string[];
        score: number;
      }
    ];
  };
}

interface Coordinates {
  latitude: number;
  longitude: number;
}

@Injectable({
  providedIn: 'root'
})
export class LocationSearchService {
  constructor(private readonly httpClient: HttpClient) {}

  public search(request: LocationSearchRequest): Observable<LocationSearchResponse> {
    if (!request.searchTerm) {
      return of({ locations: [] } as LocationSearchResponse);
    }
    return this.httpClient
      .get<ExternalSourceReponse>('https://geodata.nationaalgeoregister.nl/locatieserver/v3/free', {
        params: new HttpParams({
          fromObject: {
            q: `${request.searchTerm!} and type:adres`
          }
        })
      })
      .pipe(
        map((externalSourceReponse: ExternalSourceReponse) => {
          const response: LocationSearchResponse = {
            locations: externalSourceReponse.response.docs
              .filter(
                (externalSource) =>
                  externalSource.straatnaam &&
                  externalSource.huis_nlt &&
                  externalSource.postcode &&
                  externalSource.woonplaatsnaam
              )
              .map((externalSource) => {
                const coordinates = this.wktPointToCoordinates(externalSource.centroide_ll);
                const location: ILocationDto = {
                  displayName: externalSource.weergavenaam,
                  streetName: externalSource.straatnaam,
                  houseNumber: externalSource.huis_nlt,
                  postalCode: externalSource.postcode,
                  city: externalSource.woonplaatsnaam,
                  countryCode: 'NLD',
                  latitude: coordinates.latitude,
                  longitude: coordinates.longitude
                };
                return location;
              })
          };
          return response;
        })
      );
  }

  /*
    POINT(5.13287587 52.08884161) -> { longitude: 5.13287587, latitude: 52.08884161 }
  */
  private wktPointToCoordinates(wktPointString: string): Coordinates {
    const matches = /\(\s?(\S+)\s+(\S+)\s?\)/g.exec(wktPointString);
    return {
      longitude: matches ? parseFloat(matches[1]) : 0,
      latitude: matches ? parseFloat(matches[2]) : 0
    } as Coordinates;
  }
}
