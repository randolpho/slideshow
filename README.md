# slideshow
A Slideshow application for old tablets to be repurposed as digital photo frames


# surface/arm32 notes

## Needed dependencies via `apk add`

* libc6-compat

## QT driver issues
edit `/etc/security/pam_env.conf`

Add

    QT_QUICK_BACKEND=software
    LIBGL_ALWAYS_SOFTWARE=1
    GALLIUM_DRIVER=softpipe
