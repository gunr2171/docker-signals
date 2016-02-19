FROM mono:latest

COPY source/ /tmp/

RUN \
  xbuild /tmp/DockerSignalsExample.sln && \
  cp -r /tmp/DockerSignalsExample/bin/Debug/ /srv/program
  
ENTRYPOINT ["mono", "/srv/program/DockerSignalsExample.exe"]